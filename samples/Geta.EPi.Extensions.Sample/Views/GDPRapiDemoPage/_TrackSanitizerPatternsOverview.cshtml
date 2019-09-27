<h3>Track sanitizer pattern overview</h3>
@using (Html.BeginForm("Save", "GDPRApiDemoPage", null, FormMethod.Post, new { id = "PatternForm" }))
{
    <div class="row">
        <div class="span4">
            <div>
                <h4>Plain Text Pattern</h4>
                <textarea cols="40" rows="5" name="plaintextFilter" placeholder="Patterns are separated by line break">@Html.Raw(Model.PlainTextFilterPatterns != null ? string.Join("\r\n", Model.PlainTextFilterPatterns) : string.Empty)</textarea>
            </div>
            <div>
                <h4>Wildcard Pattern</h4>
                <textarea cols="40" rows="5" name="wildcardFilter" placeholder="Patterns are separated by line break">@Html.Raw(Model.WildcardFilterPatterns != null ? string.Join("\r\n", Model.WildcardFilterPatterns) : string.Empty)</textarea>
            </div>
            <div>
                <h4>Regex Pattern</h4>
                <textarea cols="40" rows="5" name="regexFilter" placeholder="Patterns are separated by line break">@Html.Raw(Model.RegexFilterPatterns != null ? string.Join("\r\n", Model.RegexFilterPatterns) : string.Empty)</textarea>
            </div>
            <button type="submit" class="btn btn-success">Save patterns</button>
            <div id="result"></div>
        </div>
    </div>
}

<script>
    $(function () {
        $('#PatternForm').submit(function () {
            $.ajax({
                url: this.action,
                type: this.method,
                data: $(this).serialize(),
                success: function (result) {
                    $('#result').html(`<div class="alert alert-success" id="PatternFormAlert">Patterns updated.</div>`);
                    setTimeout(function () {
                        $('#PatternFormAlert').fadeOut().remove();
                    }, 3000);
                }
            });
            return false;
        });
    });
</script>
