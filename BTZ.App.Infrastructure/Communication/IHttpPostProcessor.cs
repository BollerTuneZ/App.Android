using System;

namespace BTZ.App.Infrastructure
{
	/// <summary>
	/// Behandelt alle POST Actions
	/// Jonas Ahlf 16.04.2015 14:06:38
	/// </summary>
	public interface IHttpPostProcessor
	{
		/// <summary>
		/// Macht eine POST Anfrage an den Server und gibt 
		/// das ergebniss als string zurück
		/// </summary>
		/// <returns>Antwort vom Server in form von einem String</returns>
		/// <param name="url">URL.Addresse des Servers</param>
		/// <param name="payload">Payload.Daten, die zum Server gesendet werden sollen(wird zu JSON geparsed)</param>
		string PostAction(string url,object payload);
	}
}

