using Jibini.Polymer.Prototype.Grammar;
using Newtonsoft.Json;

namespace Jibini.Polymer.Ide
{
    public partial class CodeEditor : Form
    {
        public CodeEditor()
        {
            InitializeComponent();
        }

        private CancellationTokenSource? cancelCompile;

        private void form_Load(object? sender, EventArgs e)
        {
            var valid = new Statements(null).TryMatch(editorPane.richText.Invoke(() => editorPane.richText.Text), out var dto);
            jsonOutput.ForeColor = valid ? Color.Green : Color.Red;
            jsonOutput.Text = JsonConvert.SerializeObject(dto, Formatting.Indented);
        }

        private void editorPane_SourceChanged(object? sender, EventArgs e)
        {
            cancelCompile?.Cancel();
            cancelCompile?.Dispose();
            var cancel = (cancelCompile = new());

            Task.Run(async () =>
            {
                try
                {
                    await Task.Delay(1000, cancel.Token);
                } catch (TaskCanceledException)
                {
                    return;
                }

                var valid = new Statements(null).TryMatch(editorPane.richText.Invoke(() => editorPane.richText.Text), out var dto);
                jsonOutput.ForeColor = valid ? Color.Green : Color.Red;
                Invoke(() => jsonOutput.Text = JsonConvert.SerializeObject(dto, Formatting.Indented));
            });
        }
    }
}