﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using SonnZik.Streaming;
using SoonZik.Streaming.Singleton;


class Security
{
    static string getKey(string id)
    {
        try
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://api.lvh.me:3000/getKey/" + Singleton.Instance().TheArtiste.id.ToString());
            request.Method = "GET";
            request.ContentType = "application/json; charset=utf-8";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream());


            var test = JObject.Parse(reader.ReadToEnd()).SelectToken("key").ToString();
            return test;

        }
        catch
        {
            return null;
        }
    }


    public static String sha256_hash(String value)
    {
        StringBuilder Sb = new StringBuilder();

        using (SHA256 hash = SHA256Managed.Create())
        {
            Encoding enc = Encoding.UTF8;
            Byte[] result = hash.ComputeHash(enc.GetBytes(value));

            foreach (Byte b in result)
                Sb.Append(b.ToString("x2"));
        }

        return Sb.ToString();
    }

    public static string getSecureKey(string id)
    {
        if (Singleton.Instance().compare_date.AddMinutes(5) < DateTime.Now || Singleton.Instance().compare_date.GetHashCode() == 0)
        {
            //await new MessageDialog("GETKEY").ShowAsync();
            string key = getKey(id);
            string key_to_encode = Singleton.Instance().TheArtiste.salt + key;

            Singleton.Instance().compare_date = DateTime.Now;
            Singleton.Instance().secureKey = sha256_hash(key_to_encode);


            return Singleton.Instance().secureKey;
        }
        //await new MessageDialog("PAS DE GETKEY").ShowAsync();
        return Singleton.Instance().secureKey;
    }


}
