import { defineStore } from 'pinia'
import { ref } from 'vue'
import { familyApi, familyMemberApi } from '@/api'
import type { Family, FamilyMember, CreateFamilyMemberRequest, UpdateFamilyMemberRequest } from '@/types'

/**
 * 家族状态管理
 */
export const useFamilyStore = defineStore('family', () => {
  // 状态
  const currentFamily = ref<Family | null>(null)
  const families = ref<Family[]>([])
  const members = ref<FamilyMember[]>([])
  const loading = ref(false)

  /**
   * 加载所有家族
   */
  const loadFamilies = async () => {
    loading.value = true
    try {
      families.value = await familyApi.getAll()
    } catch (error) {
      console.error('加载家族列表失败:', error)
    } finally {
      loading.value = false
    }
  }

  /**
   * 加载家族成员
   */
  const loadMembers = async (familyId: string) => {
    loading.value = true
    try {
      members.value = await familyMemberApi.getByFamilyId(familyId)
    } catch (error) {
      console.error('加载家族成员失败:', error)
    } finally {
      loading.value = false
    }
  }

  /**
   * 设置当前家族
   */
  const setCurrentFamily = async (familyId: string) => {
    loading.value = true
    try {
      currentFamily.value = await familyApi.getById(familyId)
      await loadMembers(familyId)
    } catch (error) {
      console.error('设置当前家族失败:', error)
    } finally {
      loading.value = false
    }
  }

  /**
   * 创建家族
   */
  const createFamily = async (name: string, description?: string, originPlace?: string) => {
    loading.value = true
    try {
      const newFamily = await familyApi.create({ name, description, originPlace })
      families.value.push(newFamily)
      return newFamily
    } catch (error) {
      console.error('创建家族失败:', error)
      throw error
    } finally {
      loading.value = false
    }
  }

  /**
   * 创建成员
   */
  const createMember = async (data: CreateFamilyMemberRequest) => {
    loading.value = true
    try {
      const newMember = await familyMemberApi.create(data)
      members.value.push(newMember)
      return newMember
    } catch (error) {
      console.error('创建成员失败:', error)
      throw error
    } finally {
      loading.value = false
    }
  }

  /**
   * 更新成员
   */
  const updateMember = async (id: string, data: UpdateFamilyMemberRequest) => {
    loading.value = true
    try {
      const updatedMember = await familyMemberApi.update(id, data)
      const index = members.value.findIndex(m => m.id === id)
      if (index !== -1) {
        members.value[index] = updatedMember
      }
      return updatedMember
    } catch (error) {
      console.error('更新成员失败:', error)
      throw error
    } finally {
      loading.value = false
    }
  }

  /**
   * 删除成员
   */
  const deleteMember = async (id: string) => {
    loading.value = true
    try {
      await familyMemberApi.delete(id)
      members.value = members.value.filter(m => m.id !== id)
    } catch (error) {
      console.error('删除成员失败:', error)
      throw error
    } finally {
      loading.value = false
    }
  }

  /**
   * 保存图形布局
   */
  const saveGraphLayout = async (layout: string) => {
    if (!currentFamily.value) return
    
    try {
      await familyApi.update(currentFamily.value.id, {
        name: currentFamily.value.name,
        description: currentFamily.value.description,
        originPlace: currentFamily.value.originPlace,
        graphLayout: layout
      })
    } catch (error) {
      console.error('保存图形布局失败:', error)
      throw error
    }
  }

  return {
    currentFamily,
    families,
    members,
    loading,
    loadFamilies,
    loadMembers,
    setCurrentFamily,
    createFamily,
    createMember,
    updateMember,
    deleteMember,
    saveGraphLayout
  }
})
