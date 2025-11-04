import { http } from './http'
import type { Family, CreateFamilyRequest, UpdateFamilyRequest } from '@/types'

/**
 * 家族API
 */
export const familyApi = {
  /**
   * 获取所有家族
   */
  getAll(): Promise<Family[]> {
    return http.get('/families')
  },

  /**
   * 根据ID获取家族
   */
  getById(id: string): Promise<Family> {
    return http.get(`/families/${id}`)
  },

  /**
   * 创建家族
   */
  create(data: CreateFamilyRequest): Promise<Family> {
    return http.post('/families', data)
  },

  /**
   * 更新家族
   */
  update(id: string, data: UpdateFamilyRequest): Promise<Family> {
    return http.put(`/families/${id}`, data)
  },

  /**
   * 删除家族
   */
  delete(id: string): Promise<void> {
    return http.delete(`/families/${id}`)
  }
}
