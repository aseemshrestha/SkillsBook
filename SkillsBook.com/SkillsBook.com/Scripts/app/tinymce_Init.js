
function initTinyMce() {
    tinymce.init({
        mode: "specific_textareas",
        editor_selector: "mceEditor",
        editor_deselector: "mceNoEditor",
        height: "500",
        plugins: [
         "advlist autolink lists link image charmap print preview hr anchor pagebreak",
         "searchreplace wordcount visualblocks visualchars code fullscreen",
         "insertdatetime media nonbreaking save table contextmenu directionality",
         "emoticons template paste textcolor colorpicker textpattern imagetools"
        ],
        toolbar: "insertfile undo redo | styleselect | bold italic | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link image youtube",
        init_instance_callback: function (editor_selector) {
            editor_selector.setContent($('#con').val());
        }
            
    });
    tinymce.init({
        mode: "specific_textareas",
        editor_selector: "mceEditorQ",
        editor_deselector: "mceNoEditor",
        height: "150",
        plugins: [
         "advlist autolink lists link image charmap print preview hr anchor pagebreak",
         "searchreplace wordcount visualblocks visualchars code fullscreen",
         "insertdatetime media nonbreaking save table contextmenu directionality",
         "emoticons template paste textcolor colorpicker textpattern imagetools"
        ],
        toolbar: "insertfile undo redo | styleselect | bold italic | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link image youtube",
        init_instance_callback: function (editor_selector) {
            editor_selector.setContent($('#questionTA').val());
        }

    });
    tinymce.init({
        mode: "specific_textareas",
        editor_selector: "mceEditorComment",
        editor_deselector: "mceNoEditor",
        element:"contentComment",
        height: "300",
        plugins: [
         "advlist autolink lists link image charmap print preview hr anchor pagebreak",
         "searchreplace wordcount visualblocks visualchars code fullscreen",
         "insertdatetime media nonbreaking save table contextmenu directionality",
         "emoticons template paste textcolor colorpicker textpattern imagetools"
        ],
        toolbar: "insertfile undo redo | styleselect | bold italic | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link image youtube",
    });
    tinymce.init({
        mode: "specific_textareas",
        editor_selector: "mceEditorAnswer",
        editor_deselector: "mceNoEditor",
        element: "contentComment",
        height: "150",
        width:"700",
      
        toolbar: "undo redo | styleselect | bold italic | alignleft aligncenter alignright alignjustify",
    });
    tinymce.init({
        mode: "specific_textareas",
        editor_selector: "mceEditorCommentPopup",
        editor_deselector: "mceNoEditor",
        height: "300",
        element: "popupcomment",
        plugins: [
         "advlist autolink lists link image charmap print preview hr anchor pagebreak",
         "searchreplace wordcount visualblocks visualchars code fullscreen",
         "insertdatetime media nonbreaking save table contextmenu directionality",
         "emoticons template paste textcolor colorpicker textpattern imagetools"
        ],
        toolbar: "insertfile undo redo | styleselect | bold italic | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link image youtube",
    });
}
