using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using Dropbox.Api;
using Dropbox.Api.Files;
using System.Windows.Forms;
using POS_MoonDust.Class;
/*
 ใช้งานได้ปกติทุกอย่าง นะครับ แต่ Token หมดอายุ แล้ววววว เป็นของ Dropbox นะครับ
 */
public static class ImageUploader
{
    public static async Task<string> UploadImageToDropbox(string imagePath, string folderPath)
    {
        try
        {
            // Compress and save the image as JPEG
            var compressedImagePath = CompressImage(imagePath);

            // Upload compressed image to Dropbox
            using (var dbx = new DropboxClient(Conn.DropboxAccessToken))
            {
                var fileContent = File.ReadAllBytes(compressedImagePath);
                var folder = "/" + folderPath + "/";
                var fileName = Path.GetFileName(compressedImagePath);
                var updatedFileName = Path.Combine(folder, fileName);

                var response = await dbx.Files.UploadAsync(updatedFileName, WriteMode.Overwrite.Instance, body: new MemoryStream(fileContent));

                // Get direct link to the uploaded file
                var directLinkResponse = await dbx.Files.GetTemporaryLinkAsync(response.PathLower);
                var directLink = directLinkResponse.Link;

                // Show the direct URL in a MessageBox
                MessageBox.Show("Direct URL: " + directLink);

                // Return the URL of the file
                return directLink;
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("Error uploading image: " + ex.Message);
            return null;
        }
    }

    private static string CompressImage(string imagePath)
    {
        // Load the image
        using (var image = Image.FromFile(imagePath))
        {
            // Set compression settings
            var encoderParameters = new EncoderParameters(1);
            encoderParameters.Param[0] = new EncoderParameter(Encoder.Quality, 80L); // JPEG quality 80%

            // Compress to JPEG
            var jpegFilePath = Path.Combine(Path.GetDirectoryName(imagePath), Path.GetFileNameWithoutExtension(imagePath) + "_compressed.jpg");
            image.Save(jpegFilePath, GetEncoder(ImageFormat.Jpeg), encoderParameters);

            return jpegFilePath;
        }
    }

    private static ImageCodecInfo GetEncoder(ImageFormat format)
    {
        ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
        foreach (var codec in codecs)
        {
            if (codec.FormatID == format.Guid)
            {
                return codec;
            }
        }
        return null;
    }

}
