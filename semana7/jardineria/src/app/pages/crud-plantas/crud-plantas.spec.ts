import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CrudPlantas } from './crud-plantas';

describe('CrudPlantas', () => {
  let component: CrudPlantas;
  let fixture: ComponentFixture<CrudPlantas>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [CrudPlantas],
    }).compileComponents();

    fixture = TestBed.createComponent(CrudPlantas);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
