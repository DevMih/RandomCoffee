import { Component, Input, OnInit } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { MessageModalComponent } from 'src/app/modals/message-modal/message-modal.component';
import { Member } from 'src/app/_models/member';

@Component({
  selector: 'app-member-card',
  templateUrl: './member-card.component.html',
  styleUrls: ['./member-card.component.css']
})
export class MemberCardComponent implements OnInit {
  @Input() member: Member;
  bsModalRef: BsModalRef;
  
  constructor(private modalService: BsModalService) {}
  
  ngOnInit(): void {
 
  }

  openMessageModal() {
    const initialState = {
      member: this.member
    }
    this.bsModalRef = this.modalService.show(MessageModalComponent, {initialState});
  }
  
}