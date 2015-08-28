$(document).ready(refreshVersions);

function refreshVersions() {
    $('div[data-uri]').each(function(index, element) {
        $.getJSON('/api/proxy',
            {
                uri: element.attributes['data-uri'].value,
                responsePath: element.attributes['data-response-path'].value
            })
            .done(function(data) {
                element.innerHTML = data.Version;
                if (data.ElapsedMilliseconds > 200) {
                    $(element).parents('.panel')[0].setAttribute('class', 'panel panel-yellow');
                } else {
                    $(element).parents('.panel')[0].setAttribute('class', 'panel panel-green');
                }
            })
            .fail(function(xhr, textStatus, errorThrown) {
                element.innerHTML = errorThrown[0].toUpperCase() + errorThrown.substring(1).replace(/([a-z])(?=[A-Z])/g, '$1 ');
                $(element).parents('.panel')[0].setAttribute('class', 'panel panel-red');
            });
    });

    setTimeout(refreshVersions, 30000);
}