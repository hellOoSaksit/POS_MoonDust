using POS_MoonDust.Class;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Windows.Forms;

public static class ImgurUploader
{

    public static async Task<string> UploadImageToImgur(string imagePath)
    {
        try
        {
            // Compress and upload the image to Imgur
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Client-ID", Conn.ClientImgur);

                var form = new MultipartFormDataContent();

                using (var compressedImageStream = await CompressImage(imagePath))
                {
                    form.Add(new StreamContent(compressedImageStream), "image", "image.jpg");

                    var response = await httpClient.PostAsync("https://api.imgur.com/3/image", form);
                    response.EnsureSuccessStatusCode();

                    var responseContent = await response.Content.ReadAsStringAsync();

                    // Extract image link from the response
                    var imgurResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(responseContent);
                    var imageUrl = imgurResponse.data.link;

                    // Show the image URL in a MessageBox
                    MessageBox.Show("Image URL: " + imageUrl);

                    // Return the URL of the image
                    return imageUrl;
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("Error uploading image to Imgur: " + ex.Message);
            return null;
        }
    }

    private static async Task<Stream> CompressImage(string imagePath)
    {
        // Load the image
        using (var image = Image.FromFile(imagePath))
        {
            // Set compression settings
            var encoderParameters = new EncoderParameters(1);
            encoderParameters.Param[0] = new EncoderParameter(Encoder.Quality, 80L); // JPEG quality 80%

            // Create a memory stream to hold the compressed image
            var compressedImageStream = new MemoryStream();

            // Compress to JPEG and save to memory stream
            image.Save(compressedImageStream, GetEncoder(ImageFormat.Jpeg), encoderParameters);

            // Reset the memory stream position to the beginning
            compressedImageStream.Position = 0;

            return compressedImageStream;
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
