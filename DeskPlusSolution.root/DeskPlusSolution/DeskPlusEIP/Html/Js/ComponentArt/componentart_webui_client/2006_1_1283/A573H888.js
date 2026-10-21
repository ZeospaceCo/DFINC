
// Initialize ticker instance -------------------------------------------------

if (!(ticker_client_object))
{
  var ticker_client_object = new ComponentArt_Ticker(); 
  ticker_client_object.GlobalID = 'ticker_client_object'; 
  ticker_client_object.ElementID = 'ticker_element_id'; 
  ticker_client_object.CharDelay = char_delay; 
  ticker_client_object.LineDelay = line_delay; 
  ticker_client_object.Loop = ticker_loop; 
  ticker_client_object.Lines[0] = '...'; 
  ticker_client_object.NextTickerDelay = next_ticker_delay; 
  ticker_client_object.TickerType = ticker_type; 
  ticker_client_object.OnEnd = function () {rcr_doNothing();}; 

  rcr_StartTicker(ticker_client_object);
}