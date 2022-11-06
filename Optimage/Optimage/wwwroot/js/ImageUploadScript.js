// ---Restrict types--- //
/*
    Convert path name of current file for upload to his extension.
    Submit button activated when dependencies fo file types is confirmed.
*/

function fileSelectedChanged(image)
{
    var filePath = image.value;
    var extension = filePath.substring(filePath.lastIndexOf('.') + 1).toLowerCase();

    if (extension == 'jpg' || extension == 'png')
    {
        document.getElementById('submit-image-button').disabled = false;
    }
    else
    {
        document.getElementById('submit-image-button').disabled = 'disabled';
    }
}