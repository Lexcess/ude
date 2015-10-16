##Description

Ude can recognize the following charsets:

* UTF-8
* UTF-16 (BE and LE)
* UTF-32 (BE and LE)
* windows-1252 (mostly equivalent to iso8859-1)
* windows-1251 and ISO-8859-5 (cyrillic)
* windows-1253 and ISO-8859-7 (greek)
* windows-1255 (logical hebrew. Includes ISO-8859-8-I and most of x-mac-hebrew)
* ISO-8859-8 (visual hebrew)
* Big-5
* gb18030 (superset of gb2312)
* HZ-GB-2312
* Shift-JIS
* EUC-KR, EUC-JP, EUC-TW
* ISO-2022-JP, ISO-2022-KR, ISO-2022-CN
* KOI8-R
* x-mac-cyrillic
* IBM855 and IBM866
* X-ISO-10646-UCS-4-3412 and X-ISO-10646-UCS-4-2413 (unusual BOM)
* ASCII

## Platform
.Net Framework 4.5, Windows Phone 8, Universal Windows Apps (and while not explicitly called out should support dotnet core).

##Usage

Import the library:

using Ude;

you can feed a Stream to the detector:

using System.IO;
using Ude;

public class program
{

    public static void Main(String[] args)
    {
        ICharsetDetector cdet = new CharsetDetector();
        using (FileStream fs = File.OpenRead(filename)) 
        {
            cdet.Feed(fs);
            cdet.DataEnd();
            if (cdet.Charset != null) 
            {
                Console.WriteLine("Charset: {0}, confidence: {1}", cdet.Charset, cdet.Confidence);
            }  
            else  
            {  
                Console.WriteLine("Detection failed.");  
            } 
        }
    }
}


or use a byte array to the detector. Call DataEnd to notify the detector that you want back the result:
         
        ICharsetDetector cdet = new CharsetDetector();
        byte[] buff = new byte[1024];
        int read;
        while ((read = stream.Read(buff, 0, buff.Length)) > 0 && !done) 
        {
            cdet.Feed(buff, 0, read);
        }
        cdet.DataEnd();

        if (cdet.Charset != null) 
        {
            Console.WriteLine("Charset: {0}, confidence: {1}", cdet.Charset, cdet.Confidence);
        }  
        else  
        {  
            Console.WriteLine("Detection failed.");  
        }  


## History and other ports


This version of Ude is a fork of the C# port of Mozilla Universal Charset Detector by Rudi Pettazzi from https://code.google.com/p/ude/.

This work was based on the original source code from Mozilla available at: 

http://lxr.mozilla.org/mozilla/source/intl/chardet/src/  

The article "A composite approach to language/encoding detection" describes the algorithms of Universal Charset Detector and is available at: 

http://www-archive.mozilla.org/projects/intl/chardet.html

Some data-structures used into this port have been adapted from the Java port "juniversalchardet", available at:
     
http://code.google.com/p/juniversalchardet/

Also there is "chardet" (in Python) available at: 
        
http://chardet.feedparser.org/


Alternatively,  

    Or you can provide an alternative implementation of the interface - Ude.ICharsetDetector - 
    that wraps the original nsUniversalDetector API. 
 

##License

    This library is subject to the Mozilla Public License Version 1.1 (the "License"). An initial check of this work is available under the LGPL but subsequent versions use MPL as a sole alternative as allowed under the original terms.
