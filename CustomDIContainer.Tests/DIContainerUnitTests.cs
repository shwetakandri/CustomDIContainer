using CustomDIContainer;
using CustomDIContainer.Services;
using Xunit;

namespace CustomDIContainer.Tests
{
    public class DIContainerUnitTests
    {
        //Transient 
        [Fact]
        public void Transient_Should_Create_New_Instance()
        {
            var container = new DIContainer();

            container.Register<IEmailSender, AzureEmailSender>(ServiceLifetime.Transient);

            var service1 = container.Resolve<IEmailSender>();
            var service2 = container.Resolve<IEmailSender>();

            Assert.NotSame(service1, service2);
        }

        //Singleton 
        [Fact]
        public void Singleton_Should_Return_Same_Instance()
        {
            var container = new DIContainer();

            container.Register<IEmailSender, AzureEmailSender>(ServiceLifetime.Singleton);

            var service1 = container.Resolve<IEmailSender>();
            var service2 = container.Resolve<IEmailSender>();

            Assert.Same(service1, service2);
        }

        //Nested Dependencies 
        [Fact]
        public void Should_Resolve_Nested_Dependencies()
        {
            var container = new DIContainer();

            container.Register<IEmailSender, AzureEmailSender>(ServiceLifetime.Transient);
            container.Register<INotificationService, NotificationService>(ServiceLifetime.Transient);

            var service = container.Resolve<INotificationService>();

            Assert.NotNull(service);
            Assert.Equal("Email Sent via Azure", service.Notify());
        }

        // Validates registry of mappings from abstraction to implementation
        [Fact]
        public void Should_Return_Correct_Implementation_Type()
        {
            var container = new DIContainer();

            container.Register<IEmailSender, AzureEmailSender>(ServiceLifetime.Transient);

            var service = container.Resolve<IEmailSender>();

            Assert.IsType<AzureEmailSender>(service);
        }

        //Exception for Unregistered Service
        [Fact]
        public void Should_Throw_Exception_When_Service_Not_Registered()
        {
            var container = new DIContainer();

            Assert.Throws<InvalidOperationException>(() => container.Resolve<IEmailSender>());
        }
    }
}
