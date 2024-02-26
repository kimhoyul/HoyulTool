using System;
using System.Collections.Generic;

[Serializable]
public class WebtoonInfo
{
	public string title;
	public string pageIndicator;
	public List<string> navValues = new List<string>();
}

[Serializable]
public class WebtoonResource
{
	public List<string> _imageUrls = new List<string>();
	public Dictionary<string, string> _imageDic = new Dictionary<string, string>();
}