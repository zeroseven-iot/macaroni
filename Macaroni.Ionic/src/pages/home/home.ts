import { Component } from '@angular/core';
import { NavController } from 'ionic-angular';
import { IoTHubServiceProvider } from '../../providers/io-t-hub-service/io-t-hub-service';


@Component({
	selector: 'page-home',
	templateUrl: 'home.html',
	providers: [IoTHubServiceProvider]
})
export class HomePage {
	private messageStatus:string = "";

	constructor(public navCtrl: NavController, public iotHubService: IoTHubServiceProvider) {
	}

	public sendMessage() {
		this.iotHubService.SendMessage("Macaroni_01","test message").then(() =>{
			alert("message sent");
		});
	}
}
