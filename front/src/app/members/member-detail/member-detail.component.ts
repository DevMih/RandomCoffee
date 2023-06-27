import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { MessageModalComponent } from 'src/app/modals/message-modal/message-modal.component';
import { Member } from 'src/app/_models/member';
import { MembersService } from 'src/app/_services/members.service';

@Component({
  selector: 'app-member-detail',
  templateUrl: './member-detail.component.html',
  styleUrls: ['./member-detail.component.css']
})
export class MemberDetailComponent implements OnInit {
  member: Member;
  bsModalRef: BsModalRef;

  constructor(private memberService: MembersService, private route: ActivatedRoute,
    private modalService: BsModalService) {}

  ngOnInit(): void {
    this.loadMember();
  }

  loadMember() {
    this.memberService.getMember(this.route.snapshot.paramMap.get('username')).subscribe(member => {
      this.member = member;
    })
  }
  
  openMessageModal() {
    const initialState = {
      member: this.member
    }
    this.bsModalRef = this.modalService.show(MessageModalComponent, {initialState});
  }
}