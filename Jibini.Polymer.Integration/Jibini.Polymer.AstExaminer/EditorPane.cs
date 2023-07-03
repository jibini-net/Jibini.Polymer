using Jibini.Polymer.Prototype.Lexer;
using System.Runtime.InteropServices;

namespace Jibini.Polymer.AstExaminer;

using static Token;

public partial class EditorPane : UserControl
{
    private CancellationTokenSource? cancelTokenize;
    private string prevText = "";
    private List<TokenMatch> tokens = new();

    public EditorPane()
    {
        InitializeComponent();
    }

    private void EditorPane_Load(object sender, EventArgs e)
    {
        _TriggerHighlight();
        prevText = richText.Text;
    }

    private void richText_TextChanged(object sender, EventArgs e)
    {
        if (richText.Text != prevText)
        {
            _TriggerHighlight();
            SourceChanged.Invoke(this, EventArgs.Empty);
            prevText = richText.Text;
        }
    }

    private void _Highlight(int offset, string text, Token next)
    {
        richText.Select(offset, text.Length);
        richText.SelectionColor = next switch
        {
            Fun or Var => ColorTranslator.FromHtml("#7fc8e5"),
            For or While or If or Else => ColorTranslator.FromHtml("#e09ede"),
            Discard => ColorTranslator.FromHtml("#bbd5df"),
            Ident => ColorTranslator.FromHtml("#e7fdf6"),
            LCurly or RCurly => ColorTranslator.FromHtml("#eb67bc"),
            LParens or RParens => ColorTranslator.FromHtml("#acffff"),
            Unknown => Color.Red,
            _ => ColorTranslator.FromHtml("#ed7b7b")
        };
    }

    private void _Highlight()
    {
        richText.SuspendLayout();
        BeginUpdate();
        var oldStart = richText.SelectionStart;
        var oldLength = richText.SelectionLength;

        foreach (var tok in tokens)
        {
            _Highlight(tok.Index, tok.Text, tok.Token);
        }

        richText.Select(oldStart, oldLength);
        EndUpdate();
        richText.ResumeLayout();
        richText.ClearUndo();
    }

    private void _TriggerHighlight()
    {
        cancelTokenize?.Cancel();
        cancelTokenize?.Dispose();
        var cancel = (cancelTokenize = new());

        TokenStream source = richText.Text;
        source.SkipDiscard = false;

        Task.Run(async () =>
        {
            try
            {
                await Task.Delay(400, cancel.Token);
            } catch (TaskCanceledException)
            {
                return;
            }

            var _tokens = source.TokenizeAsync(cancel.Token);
            tokens.Clear();
            await foreach (var tok in _tokens)
            {
                tokens.Add(tok);
            }

            if (cancel.IsCancellationRequested)
            {
                return;
            }
            Invoke(() => _Highlight());
        });
    }

    public void BeginUpdate()
    {
        SendMessage(richText.Handle, WM_SETREDRAW, 0, IntPtr.Zero);
    }

    public void EndUpdate()
    {
        SendMessage(richText.Handle, WM_SETREDRAW, 1, IntPtr.Zero);
        richText.Invalidate();
    }

    [DllImport("user32.dll")]
    private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wp, IntPtr lp);
    private const int WM_SETREDRAW = 0x0b;
}
