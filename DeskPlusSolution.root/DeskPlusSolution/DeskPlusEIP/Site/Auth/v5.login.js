jQuery(document).ready(function($){
	if ($('body').hasClass('login-13')) {
		function backposition(){
			var wiha = $(window).width();
			if (wiha > 767){
				var tt = $('.login-login-box').offset().left;
				$('.leftback').css('width',tt) ;
				$('.logo').addClass('on') ;
			}else{
				$('.leftback').css('width',0) ;
			};
		}
		backposition();
		
		$(window).resize(function(){
			backposition() ;
			if (checkdoneVal == undefined ){
				$('.leftback').addClass('leftbackremoveani');
			};
			var checkdoneVal = 1;
		});	 
	}
	if ($('body').hasClass('login-14')) {
		function backposition(){
			var wiha = $(window).width();
			if (wiha > 767){
				$('.notice').addClass('on') ;
			}
		}
		backposition();
		
		$(window).resize(function(){
			backposition() ;
		});	 
	}
});
