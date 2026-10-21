
// Initialize rotator instance -------------------------------------------------

if (window.rotator_client_object) {rotator_client_object.Destroy();}

window.rotator_client_object = new ComponentArt_Rotator(); 
rotator_client_object.GlobalID = 'rotator_client_object'; 
rotator_client_object.ElementID = 'rotator_element_id';
rotator_client_object.ContainerID = 'rotator_container_id';
rotator_client_object.ContainerRowID = 'rotator_container_row_id';
rotator_client_object.AutoStart = auto_start;
rotator_client_object.SlidePause = slide_pause;
rotator_client_object.HideEffect = hide_effect; 
rotator_client_object.HideEffectDuration = hideEffect_duration;
rotator_client_object.Loop = _loop; 
rotator_client_object.PauseOnMouseOver = pause_on_mouse_over;
rotator_client_object.RotationType = rotation_type;
rotator_client_object.ScrollDirection = scroll_direction; 
rotator_client_object.ScrollInterval = scroll_interval; 
rotator_client_object.ShowEffect = show_effect; 
rotator_client_object.ShowEffectDuration = showEffect_duration;
rotator_client_object.SmoothScrollSpeed = smooth_scroll_speed;
rotator_client_object.Slides = new Array();
rotator_client_object.Slides[0] = '...'; 
rotator_client_object.HasTickers = has_tickers; 
rotator_client_object.LeadTickers[0] = '...'; 
rotator_client_object.Tickers[0] = '...'; 

if (rotator_client_object.AutoStart) {rcr_Start(rotator_client_object);}
