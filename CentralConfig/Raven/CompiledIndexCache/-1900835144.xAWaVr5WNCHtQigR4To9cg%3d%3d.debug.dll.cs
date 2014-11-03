using Raven.Abstractions;
using Raven.Database.Linq;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using System;
using Raven.Database.Linq.PrivateExtensions;
using Lucene.Net.Documents;
using System.Globalization;
using System.Text.RegularExpressions;
using Raven.Database.Indexing;


public class Index_Auto_2fBroadCastNotifyModels_2fByEventNameAndName : Raven.Database.Linq.AbstractViewGenerator
{
	public Index_Auto_2fBroadCastNotifyModels_2fByEventNameAndName()
	{
		this.ViewText = @"from doc in docs.BroadCastNotifyModels
select new { EventName = doc.EventName, Name = doc.Name }";
		this.ForEntityNames.Add("BroadCastNotifyModels");
		this.AddMapDefinition(docs => 
			from doc in docs
			where string.Equals(doc["@metadata"]["Raven-Entity-Name"], "BroadCastNotifyModels", System.StringComparison.InvariantCultureIgnoreCase)
			select new {
				EventName = doc.EventName,
				Name = doc.Name,
				__document_id = doc.__document_id
			});
		this.AddField("EventName");
		this.AddField("Name");
		this.AddField("__document_id");
		this.AddQueryParameterForMap("EventName");
		this.AddQueryParameterForMap("Name");
		this.AddQueryParameterForMap("__document_id");
		this.AddQueryParameterForReduce("EventName");
		this.AddQueryParameterForReduce("Name");
		this.AddQueryParameterForReduce("__document_id");
	}
}
