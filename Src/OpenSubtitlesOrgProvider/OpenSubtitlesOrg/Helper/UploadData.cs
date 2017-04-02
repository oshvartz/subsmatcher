﻿using SubtitleTools.Infrastructure.Core.OpenSubtitlesOrg.API;
using SubtitlesMatcher.Infrastructure;


namespace SubtitleTools.Infrastructure.Core.OpenSubtitlesOrg.Helper
{
    public class UploadData
    {
        public static string CreateUploadXml(string token,UploadBaseinfo baseinfo, UploadCDsInfo cd1)
        {
            return string.Format(
                @"<?xml version=""1.0""?>
                 <methodCall>
                 <methodName>UploadSubtitles</methodName>
                 <params>
                  <param>
                   <value><string>{0}</string></value>
                  </param>
                  <param>
                   <value>
                    <struct>
                     <member>
                      <name>baseinfo</name>
                      <value>
                       <struct>
                        <member>
                         <name>idmovieimdb</name>
                         <value><string>{1}</string></value>
                        </member>
                        <member>
                         <name>sublanguageid</name>
                         <value><string>{2}</string></value>
                        </member>
                        <member>
                         <name>moviereleasename</name>
                         <value><string>{3}</string></value>
                        </member>
                        <member>
                         <name>movieaka</name>
                         <value><string/></value>
                        </member>
                        <member>
                         <name>subauthorcomment</name>
                         <value><string/></value>
                        </member>
                       </struct>
                      </value>
                     </member>
                     <member>
                      <name>cd1</name>
                      <value>
                       <struct>
                        <member>
                         <name>subhash</name>
                         <value><string>{4}</string></value>
                        </member>
                        <member>
                         <name>subfilename</name>
                         <value><string>{5}</string></value>
                        </member>
                        <member>
                         <name>moviehash</name>
                         <value><string>{6}</string></value>
                        </member>
                        <member>
                         <name>moviebytesize</name>
                         <value><double>{7}</double></value>
                        </member>
                        <member>
                         <name>moviefps</name>
                         <value><double>0</double></value>
                        </member>
                        <member>
                         <name>movietimems</name>
                         <value><int>0</int></value>
                        </member>
                        <member>
                         <name>movieframes</name>
                         <value><int>0</int></value>
                        </member>
                        <member>
                         <name>moviefilename</name>
                         <value><string>{8}</string></value>
                        </member>
                        <member>
                         <name>subcontent</name>
                         <value><string>{9}</string></value>
                        </member>
                       </struct>
                      </value>
                     </member>
                    </struct>
                   </value>
                  </param>
                 </params>
                </methodCall>",
                              token.EscapeXml(),//{0} 
                              baseinfo.idmovieimdb.EscapeXml(),//{1}
                              baseinfo.sublanguageid.EscapeXml(),//{2}
                              baseinfo.moviereleasename.EscapeXml(),//{3}
                              cd1.subhash.EscapeXml(),//{4}
                              cd1.subfilename.EscapeXml(),//{5}
                              cd1.moviehash.EscapeXml(),//{6}
                              cd1.moviebytesize.EscapeXml(),//{7}
                              cd1.moviefilename.EscapeXml(),//{8}
                              cd1.subcontent//{9}                       
                              );
        }
    }
}
