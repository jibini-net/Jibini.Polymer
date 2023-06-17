function bindKeepIns(it) {
    var reevalBounds = () => {
        $(it).css("position", "absolute");
        $(it).css("left", "unset");
        $(it).css("top", "unset");
        $(it).css("right", "unset");
        $(it).css("bottom", "unset");
        $(it).removeClass("free");

        var within = it.closest(".keep-within");
        var originLeft = it.getBoundingClientRect().left;
        var originTop = it.getBoundingClientRect().top;
        var outOfBounds = false;

        if (it.getBoundingClientRect().right > within.getBoundingClientRect().right) {
            $(it).css("right", `${window.innerWidth - within.getBoundingClientRect().right}px`);
            outOfBounds = true;
        } else if (it.getBoundingClientRect().left < within.getBoundingClientRect().left) {
            $(it).css("left", `${within.getBoundingClientRect().left}px`);
            outOfBounds = true;
        }

        if (it.getBoundingClientRect().bottom > within.getBoundingClientRect().bottom) {
            $(it).css("bottom", `${window.innerHeight - within.getBoundingClientRect().bottom}px`);
            outOfBounds = true;
        } else if (it.getBoundingClientRect().top < within.getBoundingClientRect().top) {
            $(it).css("top", `${within.getBoundingClientRect().top}px`);
            outOfBounds = true;
        }

        if (outOfBounds) {
            $(it).addClass("free");
            if ($(it).css("top") == "unset" && $(it).css("bottom") == "unset") {
                $(it).css("top", originTop);
            }
            if ($(it).css("left") == "unset" && $(it).css("right") == "unset") {
                $(it).css("left", originLeft);
            }
        }
    };

    var within = it.closest(".keep-within");

    $(within).on("scroll", reevalBounds);

    var obs = new ResizeObserver(reevalBounds);
    obs.observe($(within)[0]);

    return ({ invoke: reevalBounds });
}

export { bindKeepIns }