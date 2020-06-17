﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Microsoft.Extensions.Logging;
using Telegrom.Core.MessageBus;
using Telegrom.Core.TelegramModel;

namespace Telegrom.Core.Contexts
{
    public sealed class SessionContext
    {
        private readonly ILifetimeScope _lifetimeScope;
        private readonly IChannelReaderProvider<Update> _incomingChannelReaderProvider;
        private readonly IChannelWriterProvider<Update> _incomingChannelWriterProvider;
        private readonly IChannelReaderProvider<Request> _outgoingChannelReaderProvider;
        private readonly IChannelWriterProvider<Request> _outgoingChannelWriterProvider;
        private readonly ITelegramRequestDispatcher _telegramRequestDispatcher;
        private readonly ISessionStateAttributesRemover _sessionStateAttributesRemover;
        private readonly ILogger<SessionContext> _logger;
        private Task _updateHandleTask;
        private Task _requestHandleTask;

        public User User { get; }

        public SessionContext(ILifetimeScope lifetimeScope,
            User user,
            IChannelReaderProvider<Update> incomingChannelReaderProvider,
            IChannelWriterProvider<Update> incomingChannelWriterProvider,
            IChannelReaderProvider<Request> outgoingChannelReaderProvider,
            IChannelWriterProvider<Request> outgoingChannelWriterProvider,
            ITelegramRequestDispatcher telegramRequestDispatcher,
            ISessionStateAttributesRemover sessionStateAttributesRemover,
            ILogger<SessionContext> logger)
        {
            _lifetimeScope = lifetimeScope;
            User = user;
            _incomingChannelReaderProvider = incomingChannelReaderProvider;
            _incomingChannelWriterProvider = incomingChannelWriterProvider;
            _outgoingChannelReaderProvider = outgoingChannelReaderProvider;
            _outgoingChannelWriterProvider = outgoingChannelWriterProvider;
            _telegramRequestDispatcher = telegramRequestDispatcher;
            _sessionStateAttributesRemover = sessionStateAttributesRemover;
            _logger = logger;
        }

        public void Start(CancellationToken cancellationToken)
        {
            async Task UpdateHandle()
            {
                await foreach (var update in _incomingChannelReaderProvider.Reader.ReadAllAsync(cancellationToken))
                {
                    var updateContext = new UpdateContext(this, update);
                    await using var innerScope = _lifetimeScope.BeginLifetimeScope(
                        typeof(UpdateContext),
                        builder => { builder.RegisterInstance(updateContext).As<IUpdateContext>(); });

                    //await innerScope.Resolve<MessageHandler>().UpdateHandleAsync(cancellationToken);
                    await innerScope.Resolve<IUpdateHandler>().Execute(cancellationToken);
                }
            }

            async Task RequestHandle()
            {
                await foreach (var request in _outgoingChannelReaderProvider.Reader.ReadAllAsync(cancellationToken))
                {
                    await _telegramRequestDispatcher.DispatchAsync(request, cancellationToken);
                }
            }

            async Task RestartOnExceptionAsync(Func<Task> task)
            {
                do
                {
                    try
                    {
                        await task();
                        break;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Get exception during update or request handle");
                    }
                } while (true);
            }

            _logger.LogInformation($"Session for identityUser {User.Id} started");

            _updateHandleTask = Task.Run(() => RestartOnExceptionAsync(UpdateHandle), cancellationToken);
            _requestHandleTask = Task.Run(() => RestartOnExceptionAsync(RequestHandle), cancellationToken);
        }

        public async Task PostUpdateAsync(Update update, CancellationToken cancellationToken)
        {
            await _incomingChannelWriterProvider.Writer.WriteAsync(update, cancellationToken);
        }

        public async Task PostRequestAsync(Request request, CancellationToken cancellationToken)
        {
            await _outgoingChannelWriterProvider.Writer.WriteAsync(request, cancellationToken);
        }

        public async Task Complete()
        {
            _incomingChannelWriterProvider.Writer.Complete();
            _outgoingChannelWriterProvider.Writer.Complete();
            if (_updateHandleTask != null)
            {
                await _updateHandleTask;
            }

            if (_requestHandleTask != null)
            {
                await _requestHandleTask;
            }

            await _sessionStateAttributesRemover.RemoveAllSessionStateAttributesAsync(User, CancellationToken.None);

            _logger.LogInformation($"Session for identityUser {User.Id} stopped");
        }
    }
}
