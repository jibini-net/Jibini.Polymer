function handleKeyDown(component, editorPane, e) {
    if (e.ctrlKey) {
        return true;
    } else {
        component.invokeMethodAsync("OnKeyPressedAsync", e.key);
        return false;
    }
}

function handlePaste(component, editorPane, e) {
    var text = (e.clipboardData || e.originalEvent.clipboardData || window.clipboardData).getData("text");
    component.invokeMethodAsync("EnterContentAsync", text);
    return false;
}

export function bindEditorEvents(component, editorPane) {
    $(editorPane).on("keydown", (e) => handleKeyDown(component, editorPane, e));
    $(editorPane).on("paste", (e) => handlePaste(component, editorPane, e));
}
