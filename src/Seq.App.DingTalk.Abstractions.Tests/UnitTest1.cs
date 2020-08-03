using System;
using Xunit;

namespace Seq.App.DingTalk.Abstractions.Tests
{
    public class Tests
    {
        [Fact]
        public void Test1()
        {
            HandlebarsHelpers.Register();

            TimeSpan.TryParse("08:00", out var offset);
            var template = "aaa {{formatDate dataTime \"yyyy-MM-dd hh:mm:ss\" \"08:00\"}} bbb";
            var ddd = HandlebarsDotNet.Handlebars.Compile(template)?.Invoke(new { dataTime = System.DateTimeOffset.Parse("2020-08-02T03:47:13.4109963+00:00") });
        }
    }
}