using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


public static class ExpandHtmlDocument
{

    public static void EachByTagName(this HtmlElement old, string tagName, Action<HtmlElement> func = null)
    {
        var hec = old.GetElementsByTagName(tagName);
        if (hec == null) return;
        foreach (HtmlElement he in hec)
        {
            if (func != null) func(he);
        }
    }

    public static string Attr(this HtmlElement old, string attrName)
    {
        return old.GetAttribute(attrName);
    }

    public static void Attr(this HtmlElement old, string attrName, string val)
    {
        old.SetAttribute(attrName, val);
    }

    /// <summary>
    /// 根据名称获得标签元素
    /// </summary>
    /// <param name="hd">HtmlDocument对象</param>
    /// <param name="name">名称</param>
    /// <returns></returns>
    public static HtmlElement GetElementByName(this HtmlDocument hd, string name)
    {
        HtmlElement he = null;

        foreach (HtmlElement item in hd.All)
        {
            if (item.TagName.ToLower().Equals("input") && item.GetAttribute("name").Equals(name))
            {
                he = item;
                break;
            }
        }
        return he;
    }

    /// <summary>
    /// 根据表单类型名称获得标签元素
    /// </summary>
    /// <param name="hd"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public static HtmlElement GetElementByType(this HtmlDocument hd, string name)
    {

        HtmlElement he = null;

        foreach (HtmlElement item in hd.All)
        {
            if (item.TagName.ToLower().Trim() == "input" && item.GetAttribute("Type").ToLower() == name.ToLower())
            {
                he = item;
                break;
            }
        }
        return he;

    }

    /// <summary>
    /// 获得提交按钮
    /// </summary>
    /// <param name="hd"></param>
    /// <param name="value">按钮显示值</param>
    /// <returns></returns>
    public static HtmlElement GetSubmit(this HtmlDocument hd, string value)
    {
        HtmlElement he = null;
        foreach (HtmlElement item in hd.All)
        {
            if (!string.IsNullOrEmpty(value))
            {
                if (item.TagName.ToLower().Trim() == "input" && item.GetAttribute("Type").ToLower() == "submit")
                {
                    string val = item.GetAttribute("value");
                    if (val.Equals(value))
                    {

                        he = item;
                        break;
                    }

                }
            }
            else
            {
                if (item.TagName.ToLower().Trim() == "input" && item.GetAttribute("Type").ToLower() == "submit")
                {
                    he = item;

                    break;
                }
            }
        }
        return he;

    }

    /// <summary>
    /// 获得元素的值
    /// </summary>
    /// <param name="he"></param>
    /// <returns></returns>
    public static string Val(this HtmlElement he)
    {
        return he.GetAttribute("value");
    }

    /// <summary>
    /// 设置元素的值
    /// </summary>
    /// <param name="he"></param>
    /// <param name="val"></param>
    public static void Val(this HtmlElement he, object val)
    {
        if (val != null)
        {
            he.SetAttribute("value", val.ToString());
        }
    }

    /// <summary>
    /// 执行单击事件
    /// </summary>
    /// <param name="he"></param>
    public static void OnClick(this HtmlElement he)
    {
        if (he == null) return;
        he.InvokeMember("click"); ;
    }

    /// <summary>
    /// 根据事件名称执行事件
    /// </summary>
    /// <param name="he"></param>
    /// <param name="EventName"></param>
    public static void OnClick(this HtmlElement he, string EventName)
    {
        he.InvokeMember(EventName); ;
    }

    /// <summary>
    /// 执行Javascript 脚本
    /// </summary>
    /// <param name="hd"></param>
    /// <param name="script">执行脚本</param>
    public static void RunJavaScript(this HtmlDocument hd, string script)
    {
        HtmlElement he = hd.CreateElement("script");
        he.SetAttribute("type", "text/javascript");
        he.SetAttribute("text", script);
        if (hd.Body != null)
        {
            hd.Body.AppendChild(he);
        }


    }

    /// <summary>
    ///  执行一个脚本方法
    /// </summary>
    /// <param name="hd"></param>
    /// <param name="functionName">方法名称</param>
    /// <param name="obj">方法参数</param>
    public static void RunJavaScriptFunction(this HtmlDocument hd, string functionName, params object[] obj)
    {
        hd.InvokeScript(functionName, obj);
    }

    /// <summary>
    /// 导入Jquery 库
    /// </summary>
    /// <param name="hd"></param>
    /// <param name="jqueryUrl">jquery 地址 可为空</param>
    public static void ImportJquery(this HtmlDocument hd, string jqueryUrl)
    {
        if (string.IsNullOrEmpty(jqueryUrl))
        {
            jqueryUrl = "http://ajax.googleapis.com/ajax/libs/jquery/1.7.2/jquery.min.js";
        }
        HtmlElement he = hd.CreateElement("script");
        he.SetAttribute("type", "text/javascript");
        he.SetAttribute("src", jqueryUrl);
        hd.Body.AppendChild(he);
    }

    /// <summary>
    /// 删除元素（需要引用 系统组建 Microsoft.mshtml）
    /// </summary>
    //public static void Remove(this HtmlElement dom)
    //{
    //    if (dom == null) return;
    //    mshtml.IHTMLDOMNode node = dom.DomElement as mshtml.IHTMLDOMNode;
    //    if (node != null)
    //    {
    //        node.parentNode.removeChild(node);
    //    }
    //}


}

/*兼容 net framework 2.0 能使用扩展方法*/
namespace System.Runtime.CompilerServices
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Assembly)]
    public sealed class ExtensionAttribute : Attribute { }

}

