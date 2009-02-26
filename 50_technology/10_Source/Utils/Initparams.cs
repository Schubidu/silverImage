using System;
using System.Linq;
using System.Windows.Browser;
using System.Collections.Generic;

namespace Utils
{
	public class Initparams
	{
		private string paramsString = HtmlPage.Plugin.GetProperty("initParams").ToString();
		private string[] paramsSplit;
		private List<Param> Params = new List<Param>();
		public Initparams()
		{
			//if(paramsString != String.Empty) {
				char[] seperator = { ';' };
				paramsSplit = paramsString.Split(seperator);
				List<string> paramList = paramsSplit.ToList<string>();
				foreach(string i in paramList){
					char[] seperatorI = {'='};
					string[] j = i.Split(seperatorI);
					Param p = new Param(j[0], j[1]);
					Params.Add(p);
				}
			//}
		}
		private class Param {
			string name = String.Empty;
			public string Name { get { return name;} set{ name = value;} }
			string initValue = String.Empty;
			public string InitValue { get { return initValue; } set { initValue = value; } }
			public Param(string name, string initValue)
			{
				Name = name;
				InitValue = initValue;
			}
		}
		public string GetSingleParam(string paramName){
			try{
				return (from Param in Params where Param.Name == paramName select Param.InitValue).ToArray<string>()[0];
			} catch (IndexOutOfRangeException ex){
				return String.Empty;
			}
		}
	}
}
