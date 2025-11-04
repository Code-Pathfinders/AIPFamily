import { http } from './http'
import type { 
  FamilyMember, 
  CreateFamilyMemberRequest, 
  UpdateFamilyMemberRequest,
  MemberPosition 
} from '@/types'

/**
 * 家族成员API
 */
export const familyMemberApi = {
  /**
   * 获取指定家族的所有成员
   */
  getByFamilyId(familyId: string): Promise<FamilyMember[]> {
    return http.get(`/familymembers/family/${familyId}`)
  },

  /**
   * 根据ID获取成员
   */
  getById(id: string): Promise<FamilyMember> {
    return http.get(`/familymembers/${id}`)
  },

  /**
   * 创建家族成员
   */
  create(data: CreateFamilyMemberRequest): Promise<FamilyMember> {
    return http.post('/familymembers', data)
  },

  /**
   * 更新家族成员
   */
  update(id: string, data: UpdateFamilyMemberRequest): Promise<FamilyMember> {
    return http.put(`/familymembers/${id}`, data)
  },

  /**
   * 删除家族成员
   */
  delete(id: string): Promise<void> {
    return http.delete(`/familymembers/${id}`)
  },

  /**
   * 批量更新成员位置
   */
  updatePositions(positions: MemberPosition[]): Promise<void> {
    return http.post('/familymembers/positions', positions)
  }
}
