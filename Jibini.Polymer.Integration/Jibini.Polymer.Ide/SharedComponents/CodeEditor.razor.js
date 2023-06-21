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

/*
https://stackoverflow.com/questions/1391278/contenteditable-change-events

$("body").on("focus", "[contenteditable]", function() {
    $(this).data("before", $(this).html());
}).on("blur keyup paste input", "[contenteditable]", function() {
    if ($(this).data("before") !== $(this).html()) {
        $(this).data("before", $(this).html());
        $(this).trigger("change");
    }
});

https://stackoverflow.com/questions/3972014/get-contenteditable-caret-position

function getCaretPosition(editableDiv) {
  var caretPos = 0, sel, range;
  if (window.getSelection) {
    sel = window.getSelection();
    if (sel.rangeCount) {
      range = sel.getRangeAt(0);
      if (range.commonAncestorContainer.parentNode == editableDiv) {
        caretPos = range.endOffset;
      }
    }
  } else if (document.selection && document.selection.createRange) {
    range = document.selection.createRange();
    if (range.parentElement() == editableDiv) {
      var tempEl = document.createElement("span");
      editableDiv.insertBefore(tempEl, editableDiv.firstChild);
      var tempRange = range.duplicate();
      tempRange.moveToElementText(tempEl);
      tempRange.setEndPoint("EndToEnd", range);
      caretPos = tempRange.text.length;
    }
  }
  return caretPos;
}
*/