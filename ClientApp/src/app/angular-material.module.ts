import { NgModule } from '@angular/core';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatSelectModule } from '@angular/material/select';


@NgModule({
  imports: [MatTableModule, MatPaginatorModule, MatSelectModule],
  exports: [MatTableModule, MatPaginatorModule, MatSelectModule]
})
export class AngularMaterialModule { }
