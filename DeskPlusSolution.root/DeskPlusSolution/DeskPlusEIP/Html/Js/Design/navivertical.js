/*!
  * navi
  * Copyright zeospace
  * Licensed zeospace
  */



$(function(){
	if($.cookie('viewtype') == '1') {
		$('.navi, .bodycontainer').addClass('icon');
	}

	$('#mnav > li >  a').mouseover(function(e) {
		$(this).parent().addClass('over').siblings().removeClass('over');
	});
	$('.navbar-menu').mouseover(function() {
		$('.navi').addClass('navbarover');
		if ($('.navi').hasClass('icon')){
			//$('.bodycontainer').stop().animate({marginLeft: '50px', opacity: '1'}, 100);
			$(this).children('a').attr('title','메뉴숨김');
			$('.dropdown-content').stop().animate({width: '220px', opacity: '1'}, 50);
		}else{
			//$('.bodycontainer').stop().animate({marginLeft: '150px', opacity: '0.4'}, 100);
			$(this).children('a').attr('title','메뉴고정')
		}

	}).mouseout(function() {
		$('.navi').removeClass('navbarover');
		if ($('.navi').hasClass('icon')){
			//$('.bodycontainer').stop().animate({marginLeft: '50px', opacity: '1'}, 100).attr('style', { 'overflow': "" });
			$('.dropdown-content').stop().animate({width: '50px', opacity: '1'}, 100);
		}else{
			$('.bodycontainer').stop().animate({marginLeft: '0px', opacity: '1'}, 100);
		}
	});



	// portlet icon change by click
	$('.portlet-roll-icon').click(function() {
		$(this).children('i').toggleClass('fa-chevron-up');
	});


	// icon view type
	$('.viewtype').click(function() {
		$('.navi, .bodycontainer').toggleClass('icon');
		$(this).children('i').toggleClass('fa-arrow-right');
		$('.viewtype').fadeTo(50, 0.1).fadeTo(50, 1.0);

		if($.cookie('viewtype') == '1') {
			$.cookie('viewtype', '0');
		}else{
			$.cookie('viewtype', '1');
		};
		console.log($.cookie('viewtype'));

	});


});
