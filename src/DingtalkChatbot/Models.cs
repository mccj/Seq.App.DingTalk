using System;

namespace DingtalkChatbot
{
    public class ActionCardBtn
    {
        public ActionCardBtn(string title, string actionURL)
        {
            Title = title;
            ActionURL = actionURL;
        }
        /// <summary>
        /// 按钮方案
        /// </summary>
        public string Title { get; }
        /// <summary>
        /// 点击按钮触发的URL
        /// </summary>
        public string ActionURL { get; }

    }
    /// <summary>
    /// FeedCard类型的消息
    /// </summary>
    public class FeedCardLinkMsg
    {
        public FeedCardLinkMsg(string title, string messageURL, string picURL)
        {
            Title = title;
            MessageURL = messageURL;
            PicURL = picURL;
        }
        /// <summary>
        /// 单条信息文本
        /// </summary>
        public string Title { get; }
        /// <summary>
        /// 点击单条信息到跳转链接
        /// </summary>
        public string MessageURL { get; }
        /// <summary>
        /// 单条信息后面图片的URL
        /// </summary>
        public string PicURL { get; }

    }
    public class ResponseInfo
    {
        /// <summary>
        /// errcode
        /// </summary>
        public long Errcode { get; set; }

        /// <summary>
        /// errmsg
        /// </summary>
        public string Errmsg { get; set; }
    }
}
