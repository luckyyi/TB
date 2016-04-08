using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TB.Helpers;
using TB.Model;

namespace TB
{
    public partial class DownloadPics : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        private List<LinkDto> GetDescTag(string htmlStr)
        {
            List<LinkDto> links = new List<LinkDto>();
            Regex regObj = new Regex("<img.*?(align=\"absmiddle\"){1}.*?>", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline);
            string[] strAry = new string[regObj.Matches(htmlStr).Count];
            int i = 0;
            foreach (Match matchItem in regObj.Matches(htmlStr))
            {
                links.Add(ConvertToDescLink(matchItem.Value));
                i++;
            }
            return links;
        }

        private List<LinkDto> GetColorsTag(string htmlStr)
        {
            List<LinkDto> links = new List<LinkDto>();
            Regex regObj = new Regex("<a.*(30x30){1}[\\s\\S]*?</a>", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline);
            string[] strAry = new string[regObj.Matches(htmlStr).Count];
            int i = 0;
            foreach (Match matchItem in regObj.Matches(htmlStr))
            {
                links.Add(ConvertToLink(matchItem.Value));
                i++;
            }
            return links;
        }

        private List<LinkDto> GetTabsTag(string htmlStr)
        {
            List<LinkDto> links = new List<LinkDto>();
            Regex regObj = new Regex("<img data-src=\"//(?<img>.*?)\"", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline);
            string[] strAry = new string[regObj.Matches(htmlStr).Count];
            int i = 0;
            foreach (Match matchItem in regObj.Matches(htmlStr))
            {
                links.Add(new LinkDto() { Img=matchItem.Groups["img"].Value });
                i++;
            }
            return links;
        }

        private LinkDto ConvertToDescLink(string str)
        {
            LinkDto link = new LinkDto();
            Regex regObj = new Regex("src=\"(?<img>.*?)\"", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            foreach (Match matchItem in regObj.Matches(str))
            {
                link.Img = matchItem.Groups["img"].Value;
            }
            return link;
        }

        private LinkDto ConvertToLink(string str)
        {
            LinkDto link = new LinkDto();
            Regex regObj = new Regex("url\\(//(?<img>.*?)\\)[\\s\\S]*<span>(?<title>.*?)</span>", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            foreach (Match matchItem in regObj.Matches(str))
            {
                link.Title = matchItem.Groups["title"].Value;
                link.Img = matchItem.Groups["img"].Value;
            }
            return link;
        }

        private string getUrlTxt(string url)
        {
            WebClient wc = new WebClient();
            string result = System.Text.Encoding.UTF8.GetString(wc.DownloadData(url));
            return result;
        }

        protected void btnDnDesc_Click(object sender, EventArgs e)
        {
            WebClient wc = new WebClient();
            string savePath = Extensions.GetConfigString("downloadImgDir").FormatWith(txtFolder.Text);
            if (!Directory.Exists(savePath))
                Directory.CreateDirectory(savePath);

            string strSource = txtDesc.Text;

            var tabs = GetTabsTag(strSource);
            var colors = GetColorsTag(strSource);
            var imgs = GetDescTag(strSource);

            int i = 0;
            tabs.ForEach(p => {
                i++;
                wc.DownloadFile("{0}{1}".FormatWith("http://", p.Img).Replace("50x50", "400x400"), "{0}\\T{1}.jpg".FormatWith(savePath, i.ToString().PadLeft(2, '0')));
            });
            colors.ForEach(p => {
                wc.DownloadFile("{0}{1}".FormatWith("http://", p.Img).Replace("30x30", "400x400"), "{0}\\{1}.jpg".FormatWith(savePath, p.Title));
            });
            int k = 0;
            imgs.ForEach(p => {
                k++;
                try
                {
                    wc.DownloadFile(p.Img, "{0}\\D{1}.jpg".FormatWith(savePath, k.ToString().PadLeft(3, '0')));
                }
                catch { }
            });

            Response.Write("图片下载完成。 主图： {0}, 颜色图：{1}, 详情图：{2} {3}".FormatWith(i, colors.Count, k, DateTime.Now.ToString("HH:mm:ss")));
        }
    }
}