import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ViewUsersTweetComponent } from './view-users-tweet.component';

describe('ViewUsersTweetComponent', () => {
  let component: ViewUsersTweetComponent;
  let fixture: ComponentFixture<ViewUsersTweetComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ViewUsersTweetComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ViewUsersTweetComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
