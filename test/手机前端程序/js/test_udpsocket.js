
	   		document.addEventListener('plusready', function(){
		   		mobile = new Mobile();
	   		});
			function send(){
				mobile.Socket.init('39.106.41.205',7777);
				//var value = $('#input').val();
				var ret = mobile.Socket.sendmessage(value);
   				switch(ret.code){
   					case -1://无网络
			 			break;
   					case 0:break;//无内容
   					case 1://收成功
   							console.log('上传成功,返回数据：'+ret.data);
							$("#msg").html($("#msg").html() + "<br />"+'上传成功,返回数据：'+ret.data);
   						break;
   					case 2://收失败
					
			 			break;
   				}
				mobile.Socket.close();
			}