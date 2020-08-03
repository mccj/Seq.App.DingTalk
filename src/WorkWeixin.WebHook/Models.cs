using System;

namespace WorkWeixinWebHook
{
    /// <summary>
    /// 图文消息，一个图文消息支持1到8条图文
    /// </summary>
    public class Article
    {
        public Article(string title, string description, string url, string picURL)
        {
            Title = title;
            Description = description;
            Url = url;
            PicURL = picURL;
        }
        /// <summary>
        /// 标题，不超过128个字节，超过会自动截断
        /// </summary>
        public string Title { get; }
        /// <summary>
        /// 描述，不超过512个字节，超过会自动截断
        /// </summary>
        public string Description { get; }
        /// <summary>
        /// 点击后跳转的链接。
        /// </summary>
        public string Url { get; }
        /// <summary>
        /// 图文消息的图片链接，支持JPG、PNG格式，较好的效果为大图 1068*455，小图150*150。
        /// </summary>
        public string PicURL { get; }
    }
    public class ResponseInfo
    {
        /// <summary>
        /// errcode
        /// </summary>
        public long ErrCode { get; set; }

        /// <summary>
        /// errmsg
        /// </summary>
        public string ErrMsg { get; set; }
    }
    public class FileResponseInfo : ResponseInfo
    {
        public string Type { get; set; }
        [Newtonsoft.Json.JsonProperty("media_id")]
        public string MediaId { get; set; }
        [Newtonsoft.Json.JsonProperty("created_at")]
        public string CreatedAt { get; set; }
    }
}
