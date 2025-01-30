using MediatR;

namespace DevFreela.Application.Notification.ProjectCreated
{
    public class FreelanceNotificationHandler : INotificationHandler<ProjectCreatedNotification>
    {
        public Task Handle(ProjectCreatedNotification notification, CancellationToken cancellationToken)
        {
            Console.WriteLine($"Notificando os freelancers sobre o projeto criado: {notification.Title}");

            return Task.CompletedTask;
        }
    }
}
