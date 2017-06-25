import { Injectable } from '@angular/core';
import { Http, RequestOptions, Headers } from '@angular/http';
import 'rxjs/add/operator/map';
import { IoTMessage } from '../../models/IoTMessage';

@Injectable()
export class IoTHubServiceProvider {

  constructor(public http: Http) {
    console.log('Hello IoTHubServiceProvider Provider');
  }

  public SendMessage(deviceName:string, text: string){

    return new Promise(resolve => {
      let message = new IoTMessage(deviceName, text);
      let body = JSON.stringify(message);
      let headers = new Headers({ 'Content-Type': 'application/json' });
      let options = new RequestOptions({ headers: headers });
      
      this.http.post('http://macaroniapi.azurewebsites.net/api/iothub/SendMessage', body, options)
            .subscribe(
              () => {
                resolve();
              }, 
              err => {
                console.error(err)
              } 
            ); 
    });
  }

}
