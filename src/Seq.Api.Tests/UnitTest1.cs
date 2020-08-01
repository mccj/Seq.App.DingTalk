using System;
using System.Linq;
using Xunit;

namespace Seq.Api.Tests
{
    public class UnitTest1
    {
        [Fact]
        public async void Test1()
        {
            var connection = new SeqConnection("http://localhost:5341", "NOOYIgdspSYBB8WuMi4C");
            var installedApps = await connection.Dashboards.ListAsync(shared:true);
           var dd= installedApps.FirstOrDefault();

           //var ddd= await connection.Data.QueryCsvAsync("select count(*) as count from stream where HealthCheckTitle = '½¡¿µ¼ì²é' and TargetUrl = 'http://10.11.2.11:8068/health' group by time(1m) having true limit 100");
        }
    }
}
