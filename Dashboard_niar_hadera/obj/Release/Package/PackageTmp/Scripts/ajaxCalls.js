function ajaxCall(method, api, data, successCB, errorCB, async = true) {
    $.ajax({
        type: method,
        url: api,
        data: data,
        cache: false,
        contentType: "application/json",
        dataType: "json",
        async: async,
        success: successCB,
        error: errorCB
    });
}