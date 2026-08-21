//生成AI使用
//WebGLでも診断データのコピペができるようにするため

mergeInto(LibraryManager.library,
{
    // 成功したら 1、失敗したら 0 を返す
    CopyToClipboard: function (text)
    {
        var str = UTF8ToString(text);

        // ===== 1段目：今までどおり自動コピーを試す =====
        var ta = document.createElement("textarea");
        ta.value = str;
        ta.setAttribute("readonly", "");   // スマホでキーボードが出ないように
        ta.style.position = "fixed";
        ta.style.top  = "0";
        ta.style.left = "0";
        ta.style.opacity = "0";            // 一瞬ちらつくのを防ぐ
        document.body.appendChild(ta);
        ta.focus();
        ta.select();
        ta.setSelectionRange(0, str.length);   // iOS Safari はこれが無いと選択されない

        var ok = false;
        try { ok = document.execCommand("copy"); }   // 戻り値を見る
        catch (e) { ok = false; }

        document.body.removeChild(ta);

        if (ok)
        {
            // 成功したことをプレイヤーに知らせる（不要ならこのブロックごと消してOK）
            var toast = document.createElement("div");
            toast.textContent = "コピーしました";
            toast.style.cssText =
                "position:fixed;left:50%;bottom:40px;transform:translateX(-50%);" +
                "background:#222;color:#fff;padding:12px 24px;border-radius:6px;" +
                "font:16px sans-serif;z-index:99999;transition:opacity .4s;";
            document.body.appendChild(toast);
            setTimeout(function(){ toast.style.opacity = "0"; }, 1200);
            setTimeout(function(){ toast.remove(); }, 1700);
            return 1;
        }

        // ===== 2段目：フォールバック。手動でコピーできる画面を出す =====
        var overlay = document.createElement("div");
        overlay.style.cssText =
            "position:fixed;inset:0;background:rgba(0,0,0,.75);z-index:99999;" +
            "display:flex;align-items:center;justify-content:center;font:14px sans-serif;";

        var box = document.createElement("div");
        box.style.cssText =
            "background:#fff;color:#000;padding:20px;border-radius:8px;" +
            "width:min(560px,90vw);display:flex;flex-direction:column;gap:12px;";

        var msg = document.createElement("div");
        msg.textContent = "下の「コピーする」ボタンを押してください。";

        var area = document.createElement("textarea");
        area.value = str;
        area.style.cssText = "width:100%;height:220px;font:12px monospace;";

        var btn = document.createElement("button");
        btn.textContent = "コピーする";
        btn.style.cssText = "padding:10px;font-size:15px;cursor:pointer;";
        btn.onclick = function ()          // 本物のクリック。ここなら確実に成功する
        {
            area.focus();
            area.select();
            area.setSelectionRange(0, str.length);
            var ok2 = false;
            try { ok2 = document.execCommand("copy"); } catch (e) {}
            msg.textContent = ok2
                ? "コピーしました。AIに貼り付けてください。"
                : "上の枠内を全選択して Ctrl+C を押してください。";
        };

        var close = document.createElement("button");
        close.textContent = "閉じる";
        close.style.cssText = "padding:8px;cursor:pointer;";
        close.onclick = function () { overlay.remove(); };

        box.appendChild(msg);
        box.appendChild(area);
        box.appendChild(btn);
        box.appendChild(close);
        overlay.appendChild(box);
        document.body.appendChild(overlay);

        return 0;
    }
});
