$(function(){
    $('#submit').click(function(){
        var refuseReasons = $.trim($("#refuseReasons").val());
        if(refuseReasons == '')
        {
            $('#refuseReasons').focus();
            return false;
        }

        $("#form-refuse").submit();
    });
   
});
