import { Observable } from 'rxjs/Rx';

export abstract class BaseService {

    //protected readonly api = process.env.API;
    protected readonly api = "/api";

    protected handleError(error: any) {

        const applicationError = error.headers['Application-Error'];

        if (applicationError) {
            return Observable.throw(applicationError);
        }

        let modelStateErrors: any = {};
        modelStateErrors.data = error.data;
        modelStateErrors.operation = error.operation;

        modelStateErrors = modelStateErrors = '' ? null : modelStateErrors;
        return Observable.throw(modelStateErrors || 'Server error');
    }
}