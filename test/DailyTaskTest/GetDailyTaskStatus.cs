using System.Text.Json;
using System.Diagnostics;
using Xunit;
using Ray.BiliBiliTool.Console;
using Microsoft.Extensions.DependencyInjection;
using Ray.BiliBiliTool.DomainService.Interfaces;
using Ray.BiliBiliTool.Infrastructure;

namespace GetDailyTaskStatusTest
{
    public class GetDailyTaskStatus
    {
        [Fact]
        public void Test1()
        {
            Program.Init(new string[] { });

            using (var scope = Global.ServiceProviderRoot.CreateScope())
            {
                var dailyTaskService = scope.ServiceProvider.GetRequiredService<IAccountDomainService>();

                var re = dailyTaskService.GetDailyTaskStatus();

                Debug.WriteLine(JsonSerializer.Serialize(re, new JsonSerializerOptions { WriteIndented = true }));

                Assert.NotNull(re);
            }
        }
    }
}
