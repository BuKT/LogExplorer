﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.XPath;

namespace LogExplorer
{
    abstract class Report
    {
        public List<LogLine> logLineList;
		public List<IReportLine> reportList;

        internal void WriteToXML(string filePath)
        {
            XmlDocument xmlDoc = new XmlDocument();
            XPathNavigator nav = xmlDoc.CreateNavigator();
            using (XmlWriter writer = nav.AppendChild())
            {
                XmlSerializer ser = new XmlSerializer(typeof(List<LogLine>), new XmlRootAttribute("TheRootElementName"));
                ser.Serialize(writer, reportList);
            }
            File.WriteAllText(filePath, xmlDoc.InnerXml);
        }

		//Сюда можно запихнуть общие для всех лайнов методы. Ну, типа компаратора, который при сортировке будет срабатывать
		public interface IReportLine {}
    }


    class UserReport : Report
    {
        public UserReport(List<LogLine> logLineList)
        {
            DateTime setTime = new DateTime(2017, 7, 6, 00, 00, 00);

            this.logLineList = logLineList;


			DateTime startTime = setTime.AddHours (-24);

			reportList = new List<IReportLine> ();
			reportList.Add (UserReportLine.GetHeader());
			foreach (LogLine line in logLineList) {
				if (ll.OutDate <= startTime || ll.OutDate >= setTime) { continue; }
				reportList.Add(UserReportLine.Obtain(line));
			}
//            reportList = logLineList.FindAll(ll => );
        }
		class UserReportLine : IReportLine {
			//Я не помню, какие тут должны быть поля, даю наобум. Поправь есичо
			private string ID;
			private string IP;
			private DateTime InDate;
			private DateTime OutDate;
			private string Company;


			//Не забудь сменить, ага?
			public string ID_HEADER = "ID пользователя";
			public string INDATE_HEADER = "Дата старта";
			public string OUTDATE_HEADER = "Дата конца";
			public string IP_HEADER = "IP тачки";
			public string COMPANY_HEADER = "Компания";

			private static UserReportLine _header;

			public static UserReportLine GetHeader() {
				if (_header == null) {
					_header = new UserReportLine();
					_header.ID = ID_HEADER;
					_header.InDate = INDATE_HEADER;
					_header.OutDate = OUTDATE_HEADER;
					_header.IP = IP_HEADER;
					_header.Company = COMPANY_HEADER;
				}
				return _header;
			}

			public static UserReportLine Obtain(LogLine line) {
				UserReportLine toReturn = new UserReportLine ();
				toReturn.ID = line.ID;
				toReturn.InDate = line.InDate;
				toReturn.OutDate = line.OutDate;
				toReturn.IP = line.IP;
				toReturn.Company = line.Company;
				return toReturn;
			}
		}
    }
    class IPReport : Report
    {
        public IPReport(List<LogLine> logLineList)
        {
            this.logLineList = logLineList;
        }
    }
    class OrgReport : Report
    {
        public OrgReport(List<LogLine> logLineList)
        {
            this.logLineList = logLineList;
        }
    }
    class OrgUserReport : Report
    {
        public OrgUserReport(List<LogLine> logLineList)
        {
            this.logLineList = logLineList;
        }
    }
    class ErrorReport : Report
    {
        public ErrorReport(List<LogLine> logLineList)
        {
            this.logLineList = logLineList;
        }
    }
}