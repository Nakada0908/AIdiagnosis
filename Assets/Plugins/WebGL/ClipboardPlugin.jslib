//生成AI使用
//WenGLでも診断データのコピペができるようにするため

mergeInto(LibraryManager.library,
{
    CopyToClipboard: function (text)
    {
        var str = UTF8ToString(text);
        var textArea = document.createElement("textarea");
        textArea.value = str;
        textArea.style.top = "0";
        textArea.style.left = "0";
        textArea.style.position = "fixed";
        document.body.appendChild(textArea);
        textArea.focus();
        textArea.select();

        try
        {
            document.execCommand('copy');
        }
        catch (err)
        {
            console.error("Copy failed.", err);
        }

        document.body.removeChild(textArea);
    }
});