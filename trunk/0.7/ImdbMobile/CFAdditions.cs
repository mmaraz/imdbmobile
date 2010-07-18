using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

public static class Extensions
{
    public static Size GetBitmapDimensions(string Name)
    {
        string File = "";
        if (System.IO.File.Exists(ImdbMobile.IMDBData.SettingsWrapper.ApplicationPath + "\\Skins\\" + Name + ".png"))
        {
            File = ImdbMobile.IMDBData.SettingsWrapper.ApplicationPath + "\\Skins\\" + Name + ".png";
        }
        else if (System.IO.File.Exists(ImdbMobile.IMDBData.SettingsWrapper.ApplicationPath + "\\Skins\\" + Name + ".jpg"))
        {
            File = ImdbMobile.IMDBData.SettingsWrapper.ApplicationPath + "\\Skins\\" + Name + ".jpg";
        }
        else
        {
            return new Size(0,0);
        }
        Bitmap b = new Bitmap(File);
        return b.Size;
    }

    public static Bitmap LoadBitmap(string Name)
    {
        if (System.IO.File.Exists(ImdbMobile.IMDBData.SettingsWrapper.ApplicationPath + "\\Skins\\" + Name + ".png"))
        {
            return new Bitmap(ImdbMobile.IMDBData.SettingsWrapper.ApplicationPath + "\\Skins\\" + Name + ".png");
        }
        else if (System.IO.File.Exists(ImdbMobile.IMDBData.SettingsWrapper.ApplicationPath + "\\Skins\\" + Name + ".jpg"))
        {
            return new Bitmap(ImdbMobile.IMDBData.SettingsWrapper.ApplicationPath + "\\Skins\\" + Name + ".jpg");
        }
        else
        {
            return new Bitmap(0, 0);
        }
    }

    public static void DrawBitmap(Graphics g, Rectangle DestRect, string Name)
    {
        string File = "";
        if (System.IO.File.Exists(ImdbMobile.IMDBData.SettingsWrapper.ApplicationPath + "\\Skins\\" + Name + ".png"))
        {
            File = ImdbMobile.IMDBData.SettingsWrapper.ApplicationPath + "\\Skins\\" + Name + ".png";
        }
        else if (System.IO.File.Exists(ImdbMobile.IMDBData.SettingsWrapper.ApplicationPath + "\\Skins\\" + Name + ".jpg"))
        {
            File = ImdbMobile.IMDBData.SettingsWrapper.ApplicationPath + "\\Skins\\" + Name + ".jpg";
        }
        else
        {
            return;
        }

        IntPtr gHdc = g.GetHdc();
        OpenNETCF.Drawing.Imaging.ImagingFactoryClass ifc = new OpenNETCF.Drawing.Imaging.ImagingFactoryClass();
        OpenNETCF.Drawing.Imaging.IImage image;
        ifc.CreateImageFromFile(File, out image);
        Size s;
        image.GetPhysicalDimension(out s);
        image.Draw(gHdc, new OpenNETCF.Drawing.Imaging.RECT(DestRect), new OpenNETCF.Drawing.Imaging.RECT(0, 0, s.Width, s.Height));
        g.ReleaseHdc(gHdc);
    }

    public static bool TryParse(string Str, out Double Dbl)
    {
        try
        {
            Dbl = Double.Parse(Str);
        }
        catch (Exception)
        {
            Dbl = 0;
        }
        if (Dbl > 0)
        {
            return true;
        }
        return false;
    }

    public static bool TryParse(string Str, out Int32 Dbl)
    {
        try
        {
            Dbl = Int32.Parse(Str);
        }
        catch (Exception)
        {
            Dbl = 0;
        }
        if (Dbl > 0)
        {
            return true;
        }
        return false;
    }

    public static bool TryParse(string Str, out Int64 Dbl)
    {
        try
        {
            Dbl = Int64.Parse(Str);
        }
        catch (Exception)
        {
            Dbl = 0;
        }
        if (Dbl > 0)
        {
            return true;
        }
        return false;
    }

    public static Image Resize(Image image, Size size)
    {

        Image bmp = new Bitmap(size.Width, size.Height);

        using (var g = Graphics.FromImage(bmp))
        {

            g.DrawImage(

                image,

                new Rectangle(0, 0, size.Width, size.Height),

                new Rectangle(0, 0, image.Width, image.Height),

                GraphicsUnit.Pixel);

        }

        return bmp;

    }

    public static SizeF MeasureStringExtended(Graphics g, string text, Font font, int desWidth)
    {
        string tempString = text;
        string workString = "";
        string outputstring = "";
        int npos = 1;
        int sp_pos = 0;
        int sp_pos1 = 0;
        int sp_pos2 = 0;
        int sp_pos3 = 0;
        bool bNeeded = false;
        int line = 0;
        int nWidth = 0;

        try
        {
            //get original size
            SizeF size = g.MeasureString(text, font);

            if (size.Width > desWidth)
            {
                while (tempString.Length > 0)
                {
                    //Check for the last lane
                    if (npos >= tempString.Length)
                    {
                        outputstring = outputstring + tempString;
                        line++;
                        break;
                    }
                    workString = tempString.Substring(0, npos);
                    //get the current width
                    nWidth = (int)g.MeasureString(workString, font).Width;
                    //check if we've got out of the destWidth
                    if (nWidth > desWidth)
                    {
                        //try to find a space

                        sp_pos1 = workString.LastIndexOf(" ");
                        sp_pos2 = workString.LastIndexOf(";");
                        sp_pos3 = workString.IndexOf("\r\n");
                        if (sp_pos3 > 0)
                        {
                            sp_pos = sp_pos3;
                            bNeeded = false;
                        }
                        else
                        {
                            if ((sp_pos2 > 0) && (sp_pos2 > sp_pos1))
                            {
                                sp_pos = sp_pos2;
                                bNeeded = true;
                            }
                            else if (sp_pos1 > 0)
                            {
                                sp_pos = sp_pos1;
                                bNeeded = true;
                            }
                            else
                            {
                                sp_pos = 0;
                                bNeeded = true;
                            }
                        }

                        if (sp_pos > 0)
                        {
                            //cut out the wrap lane we've found
                            outputstring = outputstring + tempString.Substring(0, sp_pos + 1);
                            if (bNeeded) outputstring = outputstring + "\r\n";
                            tempString = tempString.Substring((sp_pos + 1), tempString.Length - (sp_pos + 1));
                            line++;
                            npos = 0;
                        }
                        else //no space
                        {
                            outputstring = outputstring + tempString.Substring(0, npos + 1);
                            if (bNeeded) outputstring = outputstring + "\r\n";
                            tempString = tempString.Substring(npos, tempString.Length - npos);
                            line++;
                            npos = 0;

                        }
                    }
                    npos++;
                }
            }
            else
            {
                outputstring = text;
            }

            SizeF returnSize = g.MeasureString(outputstring, font);
            return returnSize;
        }
        catch (Exception e)
        {
            return new SizeF();
        }
    }
}