mergeInto(LibraryManager.library, {
    CopyToClipboard: function (str)
    {
        var text = UTF8ToString(str);
        //ブラウザのClipboardAPIまたはテキストエリアを用いたコピー処理
        if (navigator.clipboard)
        {
            navigator.clipboard.writeText(text);
        }
        else
        {
            var textArea = document.createElement("textarea");
            textArea.value = text;
            document.body.appendChild(textArea);
            textArea.select();
            document.execCommand('copy');
            document.body.removeChild(textArea);
        }
    }
});