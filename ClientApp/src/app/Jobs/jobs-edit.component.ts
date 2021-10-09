import { Component, Inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { ActivatedRoute, Router } from '@angular/router';
import { FormGroup, FormBuilder, FormControl, Validators, AbstractControl, AsyncValidatorFn } from '@angular/forms';

import { Job } from './Job';
import { API } from './../Apis/api';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

@Component({
  selector: 'app-jobs-edit',
  templateUrl: './jobs-edit.component.html',
  styleUrls: ['./jobs-edit.component.css']
})
export class JobEditComponent {

  title: string;

  form: FormGroup;

  job: Job;

  api: API;
  city: string;

  id?: number;

  apis: API[];

  constructor(
    private fb: FormBuilder,
    private activatedRoute: ActivatedRoute,
    private router: Router,
    private http: HttpClient,
    @Inject('BASE_URL') private baseUrl: string) {
  }

  ngOnInit() {

    this.form = this.fb.group({
      name:
        ['',
          Validators.required,
        ],

      description: ['',
        
          Validators.required
      ],

      momentTime: ['',
          Validators.required, 
      ],

      periodicity: ['',
        [
          Validators.required,
          Validators.pattern('[0-9]+')],    
      ],

      apiId: ['',
          Validators.required,
      ],

      city: ['',
        Validators.required,
      ],

      });
    this.loadData();
  }
  
  loadData() {

    this.loadApis();
     this.id = +this.activatedRoute.snapshot.paramMap.get('id');

    if (this.id) {

      var url = this.baseUrl + "api/jobs/" + this.id;
      this.http.get<Job>(url).subscribe(result => {
        this.job = result;
        this.title = "Edit - " + this.job.name;

        this.form.patchValue(this.job);
      }, error => console.error(error));
    }
    else {
      this.title = "Create a new Job";
    }    
  }
  

  loadApis() {
    var url = this.baseUrl + "api/apis";

    this.http.get<any>(url).subscribe(result =>
    {
      this.apis = result;
    }, error => console.error(error));
  }


  onSubmit() {

    var job = (this.id) ? this.job : <Job>{};
 
    job.name = this.form.get("name").value;
    job.description = this.form.get("description").value;
    job.momentTime = this.form.get("momentTime").value.toString();
    job.periodicity = +this.form.get("periodicity").value;
    job.apiId = +this.form.get("apiId").value;

    this.city = this.form.get("city").value;

  

    var  params = new HttpParams()
      .set('city', this.city.toString());

    var url = this.baseUrl + "api/apis/" + job.apiId;
    this.http
      .put<Text>(url, null, { params: params })
      .subscribe(result => {

        this.router.navigate(['/jobs']);
      }, error => console.error(error));


    if (this.id) {
      var url = this.baseUrl + "api/jobs/" + this.job.id;
      this.http
        .put<Job>(url, job)
        .subscribe(result => {

          console.log("Job " + job.name + " has been updated.");

          this.router.navigate(['/jobs']);
        }, error => console.error(error));
    }

    else {
      var url = this.baseUrl + "api/jobs";
      this.http
        .post<Job>(url, job)
        .subscribe(result => {
          console.log("Job" + job.id + "has been created");
          this.router.navigate(['/jobs']);
        }, error => console.error(error));
    }
  }

  isDupeField(fieldName: string): AsyncValidatorFn {
    return (control: AbstractControl): Observable<{
      [key: string]: any
    } | null> => {
      var params = new HttpParams().set("apiId",
        (this.id) ? this.id.toString() : "0").set("fieldName", fieldName).
        set("fieldValue", control.value); var url = this.baseUrl + "api/jobs/IsDupeField";
      return this.http.post<boolean>(url, null, { params }).pipe(map(result => {
        return (result ?
          { isDupeField: true } : null);
      }));
    }
  } 
}
