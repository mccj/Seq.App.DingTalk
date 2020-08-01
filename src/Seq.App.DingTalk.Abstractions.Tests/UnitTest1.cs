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

            var template = "aaa {{formatDate dataTime \"yyyy-MM-dd hh\" \"en-us\"}} bbb";
            var ddd = HandlebarsDotNet.Handlebars.Compile(template)?.Invoke(new { dataTime = System.DateTime.Now });
        }
    }
}