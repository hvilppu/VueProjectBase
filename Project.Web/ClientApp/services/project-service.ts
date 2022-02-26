import axios from 'axios';
import { BaseService } from './base-service';
import { Observable } from 'rxjs/Rx';
import { Enums } from '../enums'
import { UserDto } from '../dtos/UserDto';


class ProjectService extends BaseService {


    private static instance: ProjectService;

    public heartBeat(): any {
        return Observable.fromPromise(axios.get(this.api + '/Authentication/HeartBeat',
            { withCredentials: true }))
            .map((res: any) => res.data)
            .catch((error: any) => this.handleError(error.response));

    }

   

    private constructor() {
        super();
        axios.defaults.headers.common['Access-Control-Allow-Origin'] = '*';
        axios.defaults.headers.common['Access-Control-Allow-Methods'] = 'GET, POST, PATCH, PUT, DELETE, OPTIONS';
        axios.defaults.headers.common['Access-Control-Allow-Headers'] = 'Content-type,Accept,X-Access-Token,X-Key';
    }

    public static get Instance() {
        return this.instance || (this.instance = new this());
    }


    //** Authentication **/
    logIn(userName: string, passWord: string, workTimeEntry: boolean): any {
        return Observable.fromPromise(axios.get(this.api + '/Authentication/LogIn?username=' + userName + '&password=' + passWord + '&workTimeEntry=' + workTimeEntry,
            { withCredentials: true }))
            .map((res: any) => res.data)
            .catch((error: any) => this.handleError(error.response));
    }

    logOut(workTimeEntry: boolean): any {
        return Observable.fromPromise(axios.get(this.api + '/Authentication/LogOut?username=' + workTimeEntry,
            { withCredentials: true }))
            .map((res: any) => res.data)
            .catch((error: any) => this.handleError(error.response));
    }
}
export const projectService = ProjectService.Instance;